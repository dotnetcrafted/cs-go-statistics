import app from 'general/ts/app';
import {
    CmsIconModel,
    CmsPlayerModel,
    CmsWeaponModel,
    DurationModel
} from 'models';

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
        return ({ hours: 0, minutes: 0, seconds: 0})
    }
    
    const hours = Math.floor(duration / 60 / 60);
    const minutes = Math.floor((duration - (hours * 60 * 60)) / 60);
    const seconds = duration - (hours * 60 * 60 + minutes * 60)

    return ({
        hours,
        minutes,
        seconds
    })
}
