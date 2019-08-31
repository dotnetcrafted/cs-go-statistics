import ComponentConfig from './component-config';
import ObjectHelper from './object-helper';

export default class BaseComponent {
    constructor(element, id) {
        this.id = id;

        this._isDestroyed = false;
        this.element = element;
        this.options = {};
        this.refs = {};
        this.instanceId = null;
        this.listenersList = [];

        this.initInstanceId();
        this.initOptions();
        this.initRefs();
        this.checkRequiredRefs(this.refs);
    }

    static getRequiredRefs() {
        return [];
    }

    initOptions() {
        let options = {};
        const datasetOptionsProperty = ComponentConfig.getDatasetOptionsProperty(this.id);
        if (this.element.dataset && (datasetOptionsProperty in this.element.dataset)) {
            try {
                options = JSON.parse(this.element.dataset[datasetOptionsProperty]);
            } catch (error) {
                console.error(`Unable to parse options from ${datasetOptionsProperty} on element:`, this.element);
                throw error;
            }
        }
        this.options = options;
    }

    initRefs() {
        const refs = {};
        const selector = `[${ComponentConfig.getRefProperty(this.id)}]`;
        const refElements = this.querySelectorAll(selector);
        if (refElements.length) {
            refElements.forEach((element) => {
                const datasetRefProperty = ComponentConfig.getDatasetRefProperty(this.id);
                if (element.dataset && (datasetRefProperty in element.dataset)) {
                    ObjectHelper.addToAssociativeCollection(
                        refs,
                        element.dataset[datasetRefProperty],
                        element
                    );
                }
            });
        }
        this.refs = refs;
    }

    checkRequiredRefs(refs) {
        this.constructor.getRequiredRefs().forEach((name) => {
            if (!refs[name]) {
                throw new Error(`the value of required ref ${name} is ${refs[name]}`);
            }
        });
    }

    destroy() {
        this._isDestroyed = true;
        this.onDestroy();
    }

    isDestroyed() {
        return this._isDestroyed;
    }

    onDestroy() {
        // hook for children
    }

    initInstanceId() {
        this.instanceId = this.getNodeInstanceId(this.element, true);
    }

    getNodeInstanceId(node, component = false) {
        let result = null;

        let instanceIdDatasetProperty;
        if (component) {
            // we don't need id for root node
            instanceIdDatasetProperty = ComponentConfig.getDatasetComponentInstanceIdProperty();
        } else {
            instanceIdDatasetProperty = ComponentConfig.getDatasetInstanceIdProperty(this.id);
        }

        if (node.dataset && (instanceIdDatasetProperty in node.dataset)) {
            const instanceId = node.dataset[instanceIdDatasetProperty];
            if (instanceId) {
                result = instanceId;
            }
        }

        return result;
    }

    hasId() {
        return this.id;
    }

    getAttribute(node, attributeName) {
        if (!this.hasId()) {
            throw new Error('Id isn\'t specified');
        }

        let result = null;

        if (this.instanceId) {
            const nodeInstanceId = this.getNodeInstanceId(node);
            if (nodeInstanceId === null || this.instanceId !== nodeInstanceId) {
                return null;
            }
        }

        const attributeDatasetProperty = ComponentConfig.getDatasetAttributeProperty(this.id,
            attributeName);
        if (node.dataset && attributeDatasetProperty in node.dataset) {
            result = node.dataset[attributeDatasetProperty];
        }

        return result;
    }

    hasAttribute(node, attributeName) {
        return this.getAttribute(node, attributeName) !== null;
    }

    querySelectorAll(selector) {
        // if instance id is presented we find only refs with that id as well
        if (this.instanceId) {
            selector += `[${ComponentConfig.getInstanceIdProperty(this.id)}="${this.instanceId}"]`;
        }
        return [...this.element.querySelectorAll(selector)];
    }

    findChildrenWithAttribute(attributeName) {
        return this.querySelectorAll(`[${ComponentConfig.getAttributeProperty(this.id, attributeName)}]`);
    }

    addListener(elem, eventName, eventCallback) {
        if (!elem || typeof elem.addEventListener !== 'function') return;

        elem.addEventListener(eventName, eventCallback);
        this.listenersList.push({
            elem,
            eventName,
            eventCallback
        });
    }

    removeListeners() {
        this.listenersList.forEach((listener) => {
            const { elem, eventName, eventCallback } = listener;

            elem.removeEventListener(eventName, eventCallback);
        });

        this.listenersList = [];
    }
}
