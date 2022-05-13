import axios, { AxiosResponse } from "axios";
import { makeAutoObservable, runInAction } from "mobx";

import { CmsWeaponModel } from "../models";

export interface Weapon {
  WeaponId: number;
  Name: string;
  Type: {
    Name: string;
  }
  Image: {
    url: string;
  }
  Icon: {
    url: string
  }
}

export interface WeaponStats {
  id: number;
  headShots: number;
  headShotsRatio: number;
  kills: number;
  killsRatio: number;
}

class WeaponsStore {
  weapons: { [key: string]: CmsWeaponModel } = {};

  stats: WeaponStats[] = [];

  constructor() {
    makeAutoObservable(this);
  }

  async fetchWeaponsData() {
    try {
      const { data, status } = await axios.get<Weapon[]>(
        "https://admin.fuse8csgo.ru/weapons"
      );

      if (status === 200) {
        const weapons = {};

        data.forEach((weapon) => {
          weapons[weapon.WeaponId] = {
            id: weapon.WeaponId,
            name: weapon.Name,
            type: weapon.Type.Name,
            photoImage: weapon.Image.url,
            iconImage: weapon.Icon.url,
          };
        });

        runInAction(() => {
          this.weapons = weapons;
        });
      }
    } catch (error) {
      console.log(error);
    }
  }

  async fetchWeaponsStatsData(from: Date, to: Date) {
    try {
      const { data, status }: AxiosResponse<WeaponStats[]> = await axios.get(
        `https://fuse8csgo.ru/api/weaponsdata?dateFrom=${from.toISOString()}&dateTo=${to.toISOString()}`
      );

      if (status === 200) {
        runInAction(() => {
          this.stats = data;
        });
      }
    } catch (error) {
      console.log(error);
    }
  }
}

export default new WeaponsStore();
