import React, { ReactElement } from 'react';
import ReactDOM from 'react-dom';
import { DcBaseComponent } from '@deleteagency/dc';
import App from './app';

export default class AppComponent extends DcBaseComponent {
    static getNamespace(): string {
        return 'app';
    }

    private getAppEl(): ReactElement {
        return React.createElement(App, { playersDataUrl: this.options.playersDataUrl, wikiUrl: this.options.wikiUrl }, null);
    }

    onInit(): void {
        ReactDOM.render(this.getAppEl(), this.element);
    }
}
