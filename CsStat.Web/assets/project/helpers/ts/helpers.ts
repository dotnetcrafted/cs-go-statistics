import app from 'general/ts/app';
import { CmsIconModel, CmsPlayerModel, CmsWeaponModel } from 'models';

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
