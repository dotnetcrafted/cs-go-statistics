class ScrollbarHandler {
    constructor() {
        this.container = document.documentElement;
        this.scrollbarSize = this._getScrollbarSize();
    }

    hasScrollBar() {
        return document.body.scrollHeight > document.documentElement.clientHeight;
    }

    getScrollbarSize() {
        return this.scrollbarSize;
    }

    _getScrollbarSize() {
        const scrollDiv = document.createElement('div');
        scrollDiv.style.cssText =
            'width: 99px; height: 99px; overflow: scroll; position: absolute; top: -9999px;';
        document.body.appendChild(scrollDiv);
        const scrollbarSize = scrollDiv.offsetWidth - scrollDiv.clientWidth;
        document.body.removeChild(scrollDiv);
        return scrollbarSize;
    }
}

const instance = new ScrollbarHandler();

export default instance;
