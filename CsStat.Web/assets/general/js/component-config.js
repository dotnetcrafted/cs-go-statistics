export default class ComponentConfig {
    constructor(id, className) {
        this.id = id;
        this.className = className;
    }

    static getComponentProperty() {
        return 'data-dc-component';
    }

    static getComponentLazyProperty() {
        return 'data-dc-lazy';
    }

    static getDatasetCamelCase(attrName) {
        attrName = attrName.substr(5);
        return this.getCamelCaseSrt(attrName);
    }

    static getCamelCaseSrt(str) {
        return str.replace(/-./g, word => word.charAt(1).toUpperCase());
    }

    static getOptionsProperty(id) {
        return `data-dc-${id}-options`;
    }

    static getRefProperty(id) {
        return `data-dc-${id}-ref`;
    }

    static getComponentInstanceIdProperty() {
        return `data-dc-id`;
    }

    static getInstanceIdProperty(id) {
        return `data-dc-${id}-id`;
    }

    static getAttributeProperty(id, name) {
        return `data-dc-${id}-${name}`;
    }

    static getDatasetOptionsProperty(id) {
        return this.getDatasetCamelCase(this.getOptionsProperty(id));
    }

    static getDatasetRefProperty(id) {
        return this.getDatasetCamelCase(this.getRefProperty(id));
    }

    static getDatasetComponentInstanceIdProperty() {
        return this.getDatasetCamelCase(this.getComponentInstanceIdProperty());
    }

    static getDatasetInstanceIdProperty(id) {
        return this.getDatasetCamelCase(this.getInstanceIdProperty(id));
    }

    static getDatasetAttributeProperty(id, name) {
        return this.getDatasetCamelCase(this.getAttributeProperty(id, name));
    }
}
