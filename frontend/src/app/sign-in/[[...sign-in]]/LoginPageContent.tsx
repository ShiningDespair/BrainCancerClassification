"use client";

import React from 'react';
import { SignIn } from '@clerk/nextjs';
import styles from './LoginPageContent.module.css';

const LoginPage = () => {
  return (
    <div className={`min-h-screen flex items-center justify-center p-4 ${styles.gradientBg}`}>
      <div className="w-full max-w-md rounded-2xl bg-secondary/30 backdrop-blur-xl border border-white/10 shadow-2xl overflow-hidden">
        <div className="p-2 sm:p-4">
          <SignIn
            path="/sign-in"
            routing="path"
            signUpUrl="/sign-up"
            appearance={{
              variables: {
                colorPrimary: '#38BDF8', // accent renginiz
                colorText: '#F1F5F9', // text-primary
                colorBackground: 'transparent', 
                colorInputBackground: '#0F172A/50', // primary/50
                colorInputText: '#F1F5F9',
                borderRadius: '0.5rem',
              },
              elements: {
                card: "bg-transparent shadow-none",
                headerTitle: "text-text-primary text-2xl font-bold",
                headerSubtitle: "text-text-secondary text-sm",
                socialButtonsBlockButton: "border border-border hover:bg-secondary/80 text-text-primary",
                dividerLine: "bg-border",
                formFieldInput: "bg-primary/50 border border-border text-text-primary focus:ring-accent-light",
                footerActionLink: "text-accent-light hover:text-accent",
                formButtonPrimary: "bg-gradient-to-r from-accent-light to-accent hover:from-accent hover:to-accent-light",
              },
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default LoginPage;