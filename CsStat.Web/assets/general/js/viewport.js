const DEBOUNCE_DELAY = 150;

class Viewport {
    constructor() {
        this.resizeCallbacks = [];
        this.debounceTimeoutId = null;
        this.width = 0;
        this.height = 0;

        this.updateViewportData();
        window.addEventListener('resize', this.onWindowResize.bind(this));
    }

    getViewportWidth() {
        return this.width;
    }

    getViewportHeight() {
        return this.height;
    }

    onWindowResize() {
        if (this.debounceTimeoutId !== null) {
            clearTimeout(this.debounceTimeoutId);
        }

        this.debounceTimeoutId = setTimeout(this.handleWindowResize.bind(this), DEBOUNCE_DELAY);
    }

    updateViewportData() {
        this.width = window.outerWidth;
        // todo check difference with window.outerHeight
        this.height = document.documentElement.clientHeight;
    }

    handleWindowResize() {
        this.updateViewportData();
        this.invokeResize();
    }

    invokeResize() {
        this.resizeCallbacks.forEach(cb => cb());
    }

    subscribeOnResize(cb) {
        this.resizeCallbacks.push(cb);

        // return unsubscribe method
        return () => {
            this.resizeCallbacks = this.resizeCallbacks.filter(storedCb => storedCb !== cb);
        };
    }

    isCoordinateWithin(coordinate) {
        return this.getViewportTop() <= coordinate && this.getViewportBottom() >= coordinate;
    }

    getViewportTop() {
        return window.pageYOffset;
    }

    getViewportBottom() {
        return this.getViewportTop() + this.getViewportHeight();
    }

    isCoordinateAbove(coordinate) {
        return this.getViewportTop() >= coordinate;
    }
}

const instance = new Viewport();
export default instance;
