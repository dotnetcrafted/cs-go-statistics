
import qs from 'query-string';

const log = (...args: any[]): void => {
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
function stringFormat(template: string, ...strings: any[]): string {
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
function stringInterpolate(template: string, data: any, customizer: (key: any, dataKey: any) => string): string {
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

const getHeadshotsString = (HeadShot: number, Kills: number): string => (Kills === 0 ? '0' : `${Math.round((HeadShot / Kills) * 100)}% (${HeadShot})`);

const getHeadshotsPercent = (HeadShot: number, Kills: number): number => (Kills === 0 ? 0 : Math.round((HeadShot / Kills) * 100));

const getUrlSearch = (newValues: object, oldString: string): string => {
    const oldValues = qs.parse(oldString);
    const assigned = { ...oldValues, ...newValues };
    return qs.stringify(assigned);
};

export default {
    log,
    stringFormat,
    stringInterpolate,
    getHeadshotsString,
    getHeadshotsPercent,
    getUrlSearch
};
