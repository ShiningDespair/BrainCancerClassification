"use client";

import React from "react";
import Image from 'next/image';
import Link from "next/link";
import { useUser, SignedIn, SignedOut, UserButton } from '@clerk/nextjs';

const Header = () => {
  const { user, isLoaded } = useUser();

  return (
    <header className="fixed w-full top-0 z-50 
                       bg-black/75 
                       backdrop-blur-md 
                       border-b border-white/10 
                       shadow-lg transition-all duration-300">
      <div className="container mx-auto flex justify-between items-center px-6 py-2">
        <div className="flex items-center space-x-4">
          <Image
            src={'https://upload.wikimedia.org/wikipedia/tr/archive/9/92/20200406222625%21Istanbul_Universitesi.png'} 
            width={64}
            height={18}
            alt={'İstanbul Üniversitesi Logo'} 
            className='relative'
            priority 
          />
          <div className="text-lg font-bold text-slate-100">
            <Link href="/">MR Görüntüleri ile Hastalık Tahmini</Link>
          </div>
        </div>

        {/* Navigasyon Linkleri */}
        <nav className="flex items-center space-x-6 text-slate-300">
          <Link href="/" className="hover:text-white transition-colors">
            Anasayfa
          </Link>
          <Link href="/hakkinda" className="hover:text-white transition-colors">
            Hakkında
          </Link>
          <Link href="/iletisim" className="hover:text-white transition-colors">
            İletişim
          </Link>
          
          {/* Giriş yapmış kullanıcılar için Dashboard linki */}
          <SignedIn>
            <Link href="/dashboard" className="hover:text-white transition-colors">
              Dashboard
            </Link>
          </SignedIn>
          
          {/* Authentication Section */}
          <div className="flex items-center space-x-4">
            {!isLoaded ? (
              // Loading skeleton
              <div className="w-8 h-8 bg-slate-700 rounded-full animate-pulse"></div>
            ) : (
              <>
                <SignedIn>
                  {/* Kullanıcı giriş yapmışsa */}
                  <div className="flex items-center space-x-3">
                    <span className="text-slate-300 text-sm hidden sm:block">
                      Hoş geldin, {user?.firstName || 'Kullanıcı'}
                    </span>
                    {/* Clerk'ın hazır UserButton komponenti */}
                    <UserButton 
                      afterSignOutUrl="/"
                      appearance={{
                        elements: {
                          avatarBox: "w-8 h-8 ring-2 ring-blue-400/30 hover:ring-blue-400/50 transition-all",
                          userButtonPopoverCard: "bg-gray-900 border border-white/10",
                          userButtonPopoverActions: "text-slate-300",
                        }
                      }}
                    />
                  </div>
                </SignedIn>

                <SignedOut>
                  {/* Kullanıcı giriş yapmamışsa */}
                  <div className="flex items-center space-x-3">
                    <Link 
                      href="/sign-in"
                      className="hover:text-white transition-colors"
                    >
                      Giriş Yap
                    </Link>
                    <Link 
                      href="/sign-up"
                      className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg transition-all duration-300 text-sm"
                    >
                      Kayıt Ol
                    </Link>
                  </div>
                </SignedOut>
              </>
            )}
          </div>
        </nav>
      </div>
    </header>
  );
};

export default Header;