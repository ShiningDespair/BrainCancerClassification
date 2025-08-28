'use client'
import React from 'react'
import Link from 'next/link'
import Image from 'next/image'
import { LinkedinLogo, TwitterLogo, GithubLogo } from "@phosphor-icons/react";

const Footer = () => {
    const categories = ['MR Görüntüleri', 'Beyin Tümörü', 'MS', 'Alzheimer', ];
    const settings = {
        adres: 'Beyazıt, Fatih, İstanbul',
        mail: 'info@istanbul.edu.tr',
        telefon: '+90 212 123 45 67'
    }

    return (
        <footer className="bg-white/20 backdrop-blur-md shadow-inner rounded-t-xl mt-12">
            <div className="container mx-auto py-12 px-6">
                <div className="flex flex-wrap justify-between gap-y-8">
                    {/* Kurumsal Bilgi */}
                    <div className="basis-1/4 max-md:basis-full">
                            <Link href="/" className="flex items-center gap-2 mb-4">
                            <Image
                                src={'https://upload.wikimedia.org/wikipedia/tr/archive/9/92/20200406222625%21Istanbul_Universitesi.png'} 
                                width={64}
                                height={18}
                                alt={'İstanbul Üniversitesi Logo'} 
                                className='relative'
                                priority 
                            />
                            <span className="font-bold text-lg">İstanbul Üniversitesi</span>
                        </Link>
                        <p>{settings.adres}</p>
                        <p>E-mail: {settings.mail}</p>
                        <p>Telefon: {settings.telefon}</p>
                    </div>

                    {/* Hızlı Linkler */}
                    <div className="basis-1/4 max-md:basis-full">
                        <h3 className="font-semibold mb-2">Hızlı Linkler</h3>
                        <ul className="space-y-1">
                            <li><Link href="/">Anasayfa</Link></li>
                            <li><Link href="/about">Hakkımızda</Link></li>
                            <li><Link href="/contact">İletişim</Link></li>
                        </ul>
                    </div>

                    {/* Araçlar */}
                    <div className="basis-1/4 max-md:basis-full">
                        <h3 className="font-semibold mb-2">Araçlar</h3>
                        <ul className="space-y-1">
                            {categories.map((cat, index) => (
                                <li key={index}><Link href="#">{cat}</Link></li>
                            ))}
                        </ul>
                    </div>

                    {/* Sosyal / Abonelik */}
                    <div className="basis-1/4 max-md:basis-full">
                        <h3 className="font-semibold mb-2">Bizi Takip Edin</h3>
                        <div className="flex items-center gap-4 mb-4">
                            <Link href="https://www.linkedin.com" target="_blank"><LinkedinLogo size={28} weight="bold" /></Link>
                            <Link href="https://www.github.com" target="_blank"><GithubLogo size={28} weight="bold" /></Link>
                            <Link href="https://twitter.com" target="_blank"><TwitterLogo size={28} weight="bold" /></Link>
                        </div>
                        <div>
                            <h3 className="font-semibold mb-2">Bültene Kayıt Ol</h3>
                            <form className="flex gap-2">
                                <input type="email" placeholder="E-mail adresiniz" className="flex-1 px-3 py-2 rounded-lg border border-gray-300" required />
                                <button type="submit" className="px-4 py-2 bg-gray-600 text-white rounded-lg">Gönder</button>
                            </form>
                        </div>
                    </div>
                </div>

                <div className="mt-8 border-t border-gray-300 pt-4 text-center text-sm text-white-300">
                    ©2025 İstanbul Üniversitesi - MR Görüntüleri ile Hastalık Tahmini. Tüm hakları saklıdır.
                </div>
            </div>
        </footer>
    )
}

export default Footer;
