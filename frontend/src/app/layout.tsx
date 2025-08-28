// src/app/layout.tsx

import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import Header from '@/components/Header/Header'
import Footer from '@/components/footer/Footer'
import "@/assets/scss/app.scss"
import { ClerkProvider } from "@clerk/nextjs";
import { trTR } from '@clerk/localizations'

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "İstanbul Üniversitesi Makine Öğrenmeli MRI Analiz Aracı",
  description: "Bu araç çeşitli beyin mr'larındaki hastalıkları tanımak ve teşhis konulmasına yardımcı olunmak için tasarlanmıştır.",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <ClerkProvider localization={trTR}>
      <html lang="en">
        <body
          className={`${geistSans.variable} ${geistMono.variable} antialiased flex flex-col min-h-screen`}
        >
          <Header />
          <main className="flex-grow">
            {children}
          </main>
          <Footer />
        </body>
      </html>
    </ClerkProvider>
  );
}