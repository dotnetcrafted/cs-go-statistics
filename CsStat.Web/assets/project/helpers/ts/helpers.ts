import app from 'general/ts/app';

export const getWeaponById = (id: number) => {
    const weapons = app.state.weapons;

    if (!Array.isArray(weapons)) return null;

    const foundWeapon = weapons.find((weapon) => weapon.id === id);

    return foundWeapon ? foundWeapon : null;
}

export const getIconByName = (name: string) => {
    const icons = app.state.icons;

    if (!Array.isArray(icons)) return null;

    const foundIcon = icons.find((icon) => icon.name === name);

    return foundIcon ? foundIcon : null;
}