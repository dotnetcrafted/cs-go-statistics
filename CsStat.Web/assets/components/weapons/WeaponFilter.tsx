import React from "react";

import { PeriodType } from "./types";

interface WeaponsFilterProps {
  period?: PeriodType;
  onChange?: (period: PeriodType) => void;
}

export const WeaponFilter: React.FC<WeaponsFilterProps> = ({
  period = "week",
  onChange = () => {},
}) => {
  return (
    <div style={{ display: "flex", gap: "16px" }}>
      <button
        type="button"
        onClick={() => {
          onChange("week");
        }}
        style={{ backgroundColor: period === "week" ? "tomato" : "" }}
      >
        Week
      </button>
      <button
        type="button"
        onClick={() => {
          onChange("month");
        }}
        style={{ backgroundColor: period === "month" ? "tomato" : "" }}
      >
        Month
      </button>
      <button
        type="button"
        onClick={() => {
          onChange("year");
        }}
        style={{ backgroundColor: period === "year" ? "tomato" : "" }}
      >
        Year
      </button>
    </div>
  );
};
