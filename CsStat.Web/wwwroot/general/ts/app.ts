import { dcFactory } from '@deleteagency/dc';

interface IApp {
    dcFactory: typeof dcFactory;
    config: any;
    state: any;
}

class App implements IApp {
    dcFactory = dcFactory;

    config = (window as any).appConfig || {};
    state = (window as any).appState || {};

    init(): void {
        this.dcFactory.init();
    }

    getConfig(property: any, defaultValue = undefined): string {
        return property in this.config ? this.config[property] : defaultValue;
    }

    getRequiredConfig(property: any): string {
        if (!(property in this.config)) {
            throw new Error(`cannot find a property «${property}» in config`);
        }
        return this.config[property];
    }
}

const app = new App();
export default app;