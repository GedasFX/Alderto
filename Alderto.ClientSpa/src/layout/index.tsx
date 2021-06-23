import { PropsWithChildren, useEffect } from 'react';
import MobileMenu from './MobileMenu';
import TopBar from './TopBar';

export default function Layout({ children }: PropsWithChildren<unknown>) {
  useEffect(() => {
    document.documentElement.classList.add('dark');
    document.body.classList.add('main');
  }, []);

  return (
    <>
      <MobileMenu />
      <TopBar />
      {children}
    </>
  );
}
