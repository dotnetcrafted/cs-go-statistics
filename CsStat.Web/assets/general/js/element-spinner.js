export default class ElementSpinner {
    constructor(element, options = {}) {
        this.element = element;
        this.inited = false;
        this.options = { isFaded: false, mobileSmall: false, ...options };
    }

    init() {
        this.spinnerElement = document.createElement('div');
        this.spinnerElement.className = this.getClassName();
        this.element.insertAdjacentElement('beforeend', this.spinnerElement);
        this.inited = true;
    }

    getClassName() {
        return `spinner ${this.options.isFaded ? 'is-faded' : ''} ${this.options.mobileSmall ? 'spinner--mobile-small' : ''}`;
    }

    show() {
        if (!this.inited) {
            this.init();
        }

        this.spinnerElement.classList.add('is-visible');
    }


    hide() {
        if (this.inited) {
            this.spinnerElement.classList.remove('is-visible');
        }
    }
}
