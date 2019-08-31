import ObjectHelper from './object-helper';

export default class UrlHelper {
    /**
     * Builds query string by passed key-value object and concatenates it properly to the origin url
     * @param url
     * @param params
     * @param defaultParams
     * @returns {string}
     */
    static getUrlByParams(url, params, defaultParams = {}) {
        const paramsPrefix = url.indexOf('?') === -1 ? '?' : '&';
        const paramsString = UrlHelper.getQueryStringByParams(params, defaultParams);
        return `${url}` + (paramsString !== '' ? `${paramsPrefix}${paramsString}` : '');
    }

    /**
     * Build query string by the passed key-value object of the query parameters
     * The particular parameter won't be present in the final query string if its current value is equal to its default value from defaultParams
     * @param params
     * @param defaultParams
     * @returns {string}
     */
    static getQueryStringByParams(params, defaultParams = {}) {
        const searchableParams = this.getSearchableParams(params, defaultParams);
        return this.param(searchableParams);
    }

    /**
     * Filter params object by removing particular parameter if it has the value that is equal to its default value from defaultParams
     * @param params
     * @param defaultParams
     * @returns {{}}
     */
    static getSearchableParams(params, defaultParams = {}) {
        const result = {};
        for (const i in params) {
            if (!(i in defaultParams) || !this.isParametersValuesEqual(params[i], defaultParams[i])) {
                result[i] = params[i];
            }
        }
        return result;
    }

    static isParametersValuesEqual(parameter1, parameter2) {
        if (
            (Array.isArray(parameter1) && !Array.isArray(parameter2)) ||
            (!Array.isArray(parameter1) && Array.isArray(parameter2)) ||
            (ObjectHelper.isObject(parameter1) && !ObjectHelper.isObject(parameter2)) ||
            (!ObjectHelper.isObject(parameter1) && ObjectHelper.isObject(parameter2))
        ) {
            throw new Error('one of the arguments is Array or Object and another is not');
        } else if (
            (Array.isArray(parameter1) && Array.isArray(parameter2)) ||
            (ObjectHelper.isObject(parameter1) && ObjectHelper.isObject(parameter2))
        ) {
            return ObjectHelper.isEqual(parameter1, parameter2);
        }
        return parameter1 === parameter2;
    }

    /**
     * Returns the object of the query parameters of the passed location
     * @param location
     * @returns {*}
     */
    static getSearchFromLocation(location) {
        return this.getParamsFromQueryString(location.search.substring(1));
    }

    /**
     * Split the passed query string and returns key-value object of get parameters
     * @param queryString
     * @returns {{}}
     */
    static getParamsFromQueryString(queryString) {
        const str = queryString.replace(/\/$/, '') || '';
        const result = {};
        const parsedQueryParameters = [];

        str.split('&').forEach((keyValue) => {
            if (keyValue) {
                let value = keyValue.split('=');
                const key = decodeURIComponent(value[0]);
                value = value[1] ? decodeURIComponent(value[1]) : '';
                parsedQueryParameters.push([key, value]);
            }
        });
        parsedQueryParameters.forEach(([key, value]) => ObjectHelper.addToAssociativeCollection(result, key, value));

        return result;
    }

    /**
     * Returns the value of the passed parameter name of the passed location.
     * Return null if paramName isn't present in the location's query string
     *
     * @param paramName
     * @param location
     * @returns {*}
     */
    static getParamFromLocation(paramName, location) {
        const params = this.getSearchFromLocation(location);
        if (paramName in params) {
            return params[paramName];
        }

        return null;
    }

    static getFullUrlWithAppliedParams(location, applyParams) {
        const params = this.getSearchFromLocation(location);

        const finalParams = {};
        for (const key in params) {
            if (key in applyParams) {
                if (applyParams[key] !== undefined && applyParams[key] !== null) {
                    finalParams[key] = applyParams[key];
                }
            } else {
                finalParams[key] = params[key];
            }
        }

        return `${location.origin}${this.getUrlByParams(location.pathname, finalParams)}`;
    }

    /**
     jQuery param
     */
    static param(params) {
        var s = [];
        var add = function (k, v) {
            v = typeof v === 'function' ? v() : v;
            v = v === null ? '' : v === undefined ? '' : v;
            s[s.length] = encodeURIComponent(k) + '=' + encodeURIComponent(v);
        };
        var buildParams = function (prefix, obj) {
            var i, len, key;

            if (prefix) {
                if (Array.isArray(obj)) {
                    for (i = 0, len = obj.length; i < len; i++) {
                        buildParams(
                            prefix + '[' + (typeof obj[i] === 'object' && obj[i] ? i : i) + ']',
                            obj[i]
                        );
                    }
                } else if (String(obj) === '[object Object]') {
                    for (key in obj) {
                        buildParams(prefix + '[' + key + ']', obj[key]);
                    }
                } else {
                    add(prefix, obj);
                }
            } else if (Array.isArray(obj)) {
                for (i = 0, len = obj.length; i < len; i++) {
                    add(obj[i].name, obj[i].value);
                }
            } else {
                for (key in obj) {
                    buildParams(key, obj[key]);
                }
            }
            return s;
        };

        return buildParams('', params).join('&');
    }
}
