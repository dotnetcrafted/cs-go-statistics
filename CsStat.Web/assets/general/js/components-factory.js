import ComponentConfig from './component-config';

const ERROR_ELEMENT_PROPERTY = 'componentElement';
const NODE_COMPONENT_PROPERTY = '__dcComponent42';

export default class ComponentsFactory {
    static getInstance() {
        if (ComponentsFactory.instance === null) {
            ComponentsFactory.instance = new ComponentsFactory();
        }
        return ComponentsFactory.instance;
    }

    constructor() {
        if (ComponentsFactory.instance !== null) {
            return ComponentsFactory.instance;
        }

        this.registeredConfigs = [];
        this.instances = [];
    }

    define(id, className, selector) {
        const componentConfig = new ComponentConfig(id, className, selector);
        this.registeredConfigs.push(componentConfig);
    }

    defineWithCustomSelector(selector, className, id = null) {
        const componentConfig = new ComponentConfig(id, className, selector);
        this.registeredConfigs.push(componentConfig);
    }

    init(root = null) {
        this.registeredConfigs.forEach((config) => {
            try {
                const creatingPromises = this._bootstrapConfig(config, root);
                creatingPromises.forEach((creatingPromise) => {
                    creatingPromise.then(((instance) => {
                        // we are able to create the same component on the same node multiple times
                        // and we don't want to change existing ones or add repetitive to this.instances
                        if (this.instances.indexOf(instance) === -1) {
                            this.instances = [...this.instances, instance];
                        }
                    }), (error) => {
                        console.error(`Component ${config.className.name} with id "${config.id}" was not created due to error: ${error.message}`, error[ERROR_ELEMENT_PROPERTY]);
                        console.error(error);
                    });
                });
            } catch (e) {
                // ignore current config error and move to the next
                console.error(e);
            }
        });
    }

    initLazy(element) {
        const id = element.getAttribute(ComponentConfig.getComponentLazyProperty());
        if (!id) {
            throw new Error('can\'t find any component on lazy inited element');
        }

        const config = this.registeredConfigs.find(c => c.id === id);
        if (!config) {
            throw new Error('can\'t find specified component among registred');
        }

        try {
            return this._createComponentOnElement(config, element);
        } catch (error) {
            console.error(`Component ${config.className.name} with id "${config.id}" was not created due to error: ${error.message}`, error[ERROR_ELEMENT_PROPERTY]);
            console.error(error);
            throw error;
        }
    }

    destroy(root) {
        this.instances = this.instances.filter((instance) => {
            if (instance.element === root || root.contains(instance.element)) {
                this._destroyComponent(instance);
                return false;
            }

            return true;
        });
    }

    _bootstrapConfig(componentConfig, root = null) {
        if (root === null) {
            root = document.body;
        }
        let selector;

        if (componentConfig.selector) {
            selector = componentConfig.selector;
        } else if (componentConfig.id) {
            selector = `[${ComponentConfig.getComponentProperty()}="${componentConfig.id}"]`;
        } else {
            throw new Error('Id and Selector aren\'t provided');
        }

        const elements = [...root.querySelectorAll(selector)];
        const instances = [];

        elements.forEach((element) => {
            instances.push(new Promise((resolve, reject) => {
                setTimeout(() => {
                    try {
                        const instance = this._createComponentOnElement(componentConfig, element);
                        resolve(instance);
                    } catch (e) {
                        reject(e);
                    }
                }, 0);
            }));
        });

        return instances;
    }

    _createComponentOnElement(componentConfig, element) {
        const className = componentConfig.className;

        if (!element.components) {
            element.components = {};
        }

        if (element[NODE_COMPONENT_PROPERTY]) {
            return element[NODE_COMPONENT_PROPERTY];
        }
        try {
            const instance = new className(element, componentConfig.id);
            element[NODE_COMPONENT_PROPERTY] = instance;
            return instance;
        } catch (e) {
            e[ERROR_ELEMENT_PROPERTY] = element;
            throw e;
        }
    }

    _destroyComponent(component) {
        component.element.components[component.id] = null;
        component.destroy();
    }
}

ComponentsFactory.instance = null;
