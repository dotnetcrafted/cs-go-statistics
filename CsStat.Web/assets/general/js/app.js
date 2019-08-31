import dcFactory from './dc/dc-factory';

class App {
    constructor() {
        this.dcFactory = dcFactory;
        this.config = window.appConfig || {};
        this.state = window.appState || {};
    }

    init() {
        this.dcFactory.init();
    }

    getConfig(property, defaultValue = undefined) {
        return property in this.config ? this.config[property] : defaultValue;
    }

    getRequiredConfig(property) {
        if (!(property in this.config)) {
            throw new Error(`cannot find a property «${property}» in config`);
        }
        return this.config[property];
    }
}

const instance = new App();
export default instance;
