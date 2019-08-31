const log = (...args) => {
    if (window.console && window.console.log) {
        window.console.log(...args);
    }
};

/**
 * Analogue C# string format
 * stringFormat('Lorem {0} dolor {1} amet', 'ipsum', 'sit')
 * @param {String} template
 * @param {String} strings for replacing
 * @return {String} compiled template
 */
function stringFormat(template, ...strings) {
    return template.replace(/{(\d+)}/g, (match, number) =>
        (typeof strings[number] !== 'undefined' ? strings[number] : match));
}

/**
 * Example: stringFormat('Lorem {foo} dolor {bar} amet', {foo: 'Foo string', bar: 'Bar string'})
 * @param {String} template
 * @param {Object} data for replacing
 * @param {Function} customizer - an optional function that can implement custom logic for every
 *                   entry replacement within template string
 * @return {String} compiled template
 */
function stringInterpolate(template, data, customizer = null) {
    return template.replace(/{(.+?)}/g, (match, key) => {
        if (key in data) {
            if (customizer) {
                return customizer(key, data[key]);
            }
            return data[key];
        }
        return match;
    });
}


export default {
    log,
    stringFormat,
    stringInterpolate,
};
