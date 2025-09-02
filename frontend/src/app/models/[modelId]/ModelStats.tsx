"use client";

import React from "react";
import { usePathname } from "next/navigation"; // ✅ Next.js hook
import { Tag, Cube, ChartBar, CheckCircle } from "@phosphor-icons/react";

type Props = {
  stats: {
    version: string;
    inputShape: string;
    accuracy?: number | null;
    mae?: number | null;
  };
};

const formatAccuracy = (acc?: number | null) => {
  if (acc == null) return "N/A";
  return acc <= 1 ? `%${(acc * 100).toFixed(1)}` : `%${acc.toFixed(2)}`;
};

const formatMAE = (mae?: number | null) => {
  if (mae == null) return "N/A";
  return mae.toFixed(2);
};

const ModelStats = ({ stats }: Props) => {
  const pathname = usePathname(); // current path (e.g. "/models/3")

  // ✅ if URL ends with /models/3 treat as regression
  const isRegression = pathname === "/models/3";

  const statItems = [
    { icon: Tag, label: "Model Versiyonu", value: stats.version },
    { icon: Cube, label: "Girdi Boyutu", value: stats.inputShape },
  ];

  return (
    <div>
      <h2 className="text-3xl font-bold text-text-primary mb-6">
        Model İstatistikleri
      </h2>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        {statItems.map((item, index) => (
          <div
            key={index}
            className="bg-secondary/50 rounded-2xl p-6 border border-border"
          >
            <div className="flex items-center text-text-secondary mb-2">
              <item.icon size={18} className="mr-2" />
              <h3 className="text-sm font-medium">{item.label}</h3>
            </div>
            <p className="text-3xl font-semibold text-text-primary">
              {item.value}
            </p>
          </div>
        ))}

        <div className="bg-secondary/50 rounded-2xl p-6 border border-border">
          <div className="flex items-center text-text-secondary mb-2">
            <ChartBar size={18} className="mr-2" />
            <h3 className="text-sm font-medium">Toplam Analiz</h3>
          </div>
          <p className="text-3xl font-semibold text-text-primary">N/A</p>
        </div>

        <div className="bg-secondary/50 rounded-2xl p-6 border border-border">
          <div className="flex items-center text-text-secondary mb-2">
            <CheckCircle size={18} className="mr-2" />
            <h3 className="text-sm font-medium">
              {isRegression ? "Ortalama Hata (MAE)" : "Doğruluk Oranı"}
            </h3>
          </div>
          <p className="text-3xl font-semibold text-text-primary">
            {isRegression
              ? formatMAE(stats.accuracy)
              : formatAccuracy(stats.accuracy)}
          </p>
        </div>
      </div>
    </div>
  );
};

export default ModelStats;
