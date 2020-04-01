export interface CmsPlayerModel {
    nickName: string,
    steamId: string,
    steamImage: string,
    rang: number,
}

export interface CmsWeaponModel {
    id: number,
    name: string,
    type: string,
    photoImage: string,
    iconImage: string,
}

export interface CmsIconModel {
    name: string,
    image: string,
}

export interface CmsApiModel {
    name: string,
    value: string,
}
