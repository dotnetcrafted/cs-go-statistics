import React, { useState, useEffect, useCallback } from "react";
import { observer } from "mobx-react-lite";
import {
  startOfWeek,
  endOfWeek,
  startOfMonth,
  endOfMonth,
  startOfYear,
  endOfYear,
} from "date-fns";
import { Tabs } from "antd";

import { weaponsStore, WeaponStats } from "../../store";
import { TopWeapons } from "./TopWeapons";
import { WeaponFilter } from "./WeaponFilter";
import { PeriodType } from "./types";

const { TabPane } = Tabs;

const groups = [
  { name: "Rifles", types: ["Rifle", "Sniper Rifle"] },
  { name: "SMGs", types: ["Submachine Gun", "Machine Gun"] },
  { name: "Pistols", types: ["Pistol"] },
  { name: "Heavies", types: ["Shotgun"] },
  {
    name: "Others",
    types: ["Knife", "Zeus", "Explosive Grenade", "Fire Grenade"],
  },
];

export const Weapons = observer(() => {
  const [period, setPeriod] = useState<PeriodType>("week");
  const { weapons, stats: weaponStats } = weaponsStore;


  useEffect(() => {
    const today = new Date();

    let from: Date;
    let to: Date;

    switch (period) {
      case "week": {
        from = startOfWeek(today, { weekStartsOn: 1 });
        to = endOfWeek(today, { weekStartsOn: 1 });
        break;
      }
      case "month": {
        from = startOfMonth(today);
        to = endOfMonth(today);
        break;
      }
      case "year": {
        from = startOfYear(today);
        to = endOfYear(today);
        break;
      }
      default: {
        from = today;
        to = today;
      }
    }
    weaponsStore.fetchWeaponsStatsData(from, to);
  }, [period]);

  const normalisedStats: Record<string, WeaponStats> = {};

  const handleOnFilterChange = useCallback((newPeriod: PeriodType) => {
    setPeriod(newPeriod);
  }, []);

  weaponStats.forEach((weapon) => {
    normalisedStats[weapon.id] = weapon;
  });

  if (!Object.keys(normalisedStats).length) return null;

  return (
    <div style={{ background: "lightgrey" }}>
      <WeaponFilter period={period} onChange={handleOnFilterChange} />
      <TopWeapons />
      <Tabs defaultActiveKey={groups[0].name}>
        {groups.map((group) => {
          const weaponsIds = group.types.flatMap((type) => {
            const res = Object.values(weapons).filter((value) => {
              return value.type === type;
            });

            return res.map((x) => x.id);
          });

          const mappedWeapons = weaponsIds
            .map((id) => {
              const weapon = weapons[id];
              const itemStats = normalisedStats[id] ?? null;

              return { id, ...weapon, stats: itemStats };
            })
            .sort((a, b) => {
              if (b.stats === null) return -1;

              return Math.sign(Number(b.stats?.kills) - Number(a.stats?.kills));
            });

          return (
            <TabPane key={group.name} tab={group.name}>
              <div
                style={{ display: "flex", gap: "16px", textAlign: "center" }}
              >
                <div style={{ width: 200, textAlign: "left" }}>Name</div>
                <div style={{ width: 50 }}>Kills</div>
                <div style={{ width: 50 }}>Headshots</div>
              </div>
              {mappedWeapons.map(({ id, name, stats }) => {
                return (
                  <div
                    key={id}
                    style={{
                      display: "flex",
                      gap: "16px",
                      textAlign: "center",
                    }}
                  >
                    <div style={{ width: 200, textAlign: "left" }}>{name}</div>
                    <div style={{ width: 50 }}>{stats?.kills ?? 0}</div>
                    <div style={{ width: 50 }}>
                      {stats?.headShotsRatio ?? 0} %
                    </div>
                  </div>
                );
              })}
            </TabPane>
          );
        })}
      </Tabs>
    </div>
  );
});
