import app from './app';

if (!Element.prototype.matches) {
    Element.prototype.matches = Element.prototype.msMatchesSelector;
}

export default class HtmlHelper {
    static createElement(htmlString) {
        if (htmlString.indexOf('<html') !== -1) {
            throw new Error(
                'Trying to create element from the complete html page. Partial html is required'
            );
        }

        const template = document.createElement('template');
        template.innerHTML = htmlString.trim();
        const element = template.content ? template.content.firstChild : template.firstChild;
        // move element to current document to avoid bugs later when we insert it to DOM
        document.adoptNode(element);
        return element;
    }

    static getParent(target, selector, includeSelf = true) {
        if (includeSelf && target.matches(selector)) {
            return target;
        }

        let parent = target.parentElement;
        while (parent !== null) {
            if (parent.matches(selector)) {
                return parent;
            }
            parent = parent.parentElement;
        }

        return null;
    }

    static safeAddEventListener(element) {
        if (element) {
            const args = [...arguments].slice(1);
            element.addEventListener(...args);
        }
    }

    static hasElementParentWithClass(target, parentClass, includeSelf = true) {
        return this.getElementParentWithClass(target, parentClass, includeSelf) !== null;
    }

    static getElementParentWithClass(target, parentClass, includeSelf = true) {
        if (includeSelf && target.classList.contains(parentClass)) {
            return target;
        }

        let parent = target.parentElement;
        while (parent !== null) {
            if (parent.classList.contains(parentClass)) {
                return parent;
            }
            parent = parent.parentElement;
        }

        return null;
    }

    static isHtmlElement(element) {
        return typeof element === 'object' && 'nodeType' in element;
    }

    static getSrcSet(imageUrl, addSizes = []) {
        const defaultSizes = [320, 360, 640, 720, 1280, 1440];
        let sizes = [...defaultSizes];

        if (addSizes && addSizes.length > 0) {
            sizes = [...sizes, ...addSizes];
        }

        if (!imageUrl || imageUrl.length === 0) {
            imageUrl = app.getConfig('defaultImageUrl', null);
        }

        if (imageUrl === null) {
            return '';
        }

        return sizes
            .map(width => `${HtmlHelper.GetResizedMediaUrl(imageUrl, width)} ${width}w`)
            .join(', ');
    }

    static GetResizedMediaUrl(imageUrl, width) {
        if (imageUrl) {
            imageUrl = imageUrl.replace(/(~)|&?(width|height)=[0-9]+/, '');
            imageUrl = imageUrl.replace(/\?&/, '');
            imageUrl = imageUrl.replace(/(\?|&)$/, '');

            const paramsPrefix = imageUrl.indexOf('?') === -1 ? '?' : '&';
            imageUrl += paramsPrefix;
            return imageUrl + 'width=' + width;
        }

        return '';
    }

    static getElementByHash(hash) {
        const id = hash.replace('#', '');
        return document.getElementById(id);
    }
}
