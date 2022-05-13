import app from 'general/ts/app';
import {
    CmsIconModel,
    CmsPlayerModel,
    CmsWeaponModel,
    DurationModel
} from 'models';
import React from 'react';

export const getWeaponById = (id: number): CmsWeaponModel | null => {
    const weapons = app.state.weapons;

    if (!Array.isArray(weapons)) return null;

    return weapons.find((weapon) => weapon.id === id) || null;
};

export const getIconByName = (name: string): CmsIconModel | null => {
    const icons = app.state.icons;

    if (!Array.isArray(icons)) return null;

    return icons.find((icon) => icon.name === name) || null;
};

export const getPlayerById = (id: string): CmsPlayerModel | null => {
    const players = app.state.players;

    if (!Array.isArray(players)) return null;

    return players.find((player) => player.steamId === id) || null;
};

export const getDuration = (duration: number): DurationModel => {
    if (typeof duration !== 'number') {
        return { hours: 0, minutes: 0, seconds: 0 };
    }

    const hours = Math.floor(duration / 60 / 60);
    const minutes = Math.floor((duration - hours * 60 * 60) / 60);
    const seconds = duration - (hours * 60 * 60 + minutes * 60);

    return {
        hours,
        minutes,
        seconds
    };
};

const UNICODE_SPACE_CHAR = '\u00A0';

// TODO: to think about float part like 00; how to solve it
export const getTableValueByMask = (
    value: (number | string)[],
    mask: (number | string)[],
    addZero: boolean = false
): React.ReactNode => {
    if (value.length !== mask.length) {
        return value.join('');
    }

    const result = value.map((item, i) => {
        const pattern = mask[i];

        if (typeof pattern === 'string') return item;

        if (typeof pattern === 'number') {
            const patternLength = Math.abs(pattern) + (pattern < 0 ? 1 : 0);
            const itemLength =
                Math.abs(+item).toString().length + (pattern < 0 ? 1 : 0);

            const diff = patternLength - itemLength;

            let newItem: React.ReactNode = item;

            if (diff > 0) {
                newItem = `${UNICODE_SPACE_CHAR.repeat(diff)}${item}`;

                if (addZero) {
                    newItem = (
                        <span key={item}>
                            <span className="lead-zero">
                                {'0'.repeat(diff)}
                            </span>
                            <span>{item}</span>
                        </span>
                    );
                }
            }

            if (pattern < 0 && item >= 0) {
                const sign = item < 0 ? '-' : UNICODE_SPACE_CHAR;

                newItem = <span key={item}>{sign}{newItem}</span>;
            }

            return newItem;
        }

        return item;
    });

    return result;
};
