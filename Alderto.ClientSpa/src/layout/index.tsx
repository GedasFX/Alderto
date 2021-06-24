import { PropsWithChildren, useEffect } from 'react';
import MobileMenu from './MobileMenu';
import SideNav from './SideNav';
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
      <div className="wrapper">
        <div className="wrapper-box dark:bg-dark-8">
          <SideNav />
          <div className="content w-full">{children}</div>
        </div>
      </div>
    </>
  );
}
