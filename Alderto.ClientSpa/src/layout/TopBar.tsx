import clsx from 'clsx';
import Link from 'next/link';
import AccountMenu from './AccountMenu';
import Breadcrumb from './Breadcrumb';
import Logo from './Logo';
import styles from './TopBar.module.css';

export default function TopBar() {
  return (
    <div
      className={clsx(
        'border-b border-theme-2 -mt-7 md:-mt-5 -mx-3 sm:-mx-8 px-3 sm:px-8 md:pt-0 mb-12',
        styles['top-bar-boxed']
      )}
    >
      <div className="h-full flex items-center">
        <Link href="/">
          <a className="-intro-x hidden md:flex">
            <Logo />
            <span className="text-white text-lg ml-3">Alderto</span>
          </a>
        </Link>
        <Breadcrumb />
        <AccountMenu />
      </div>
    </div>
  );
}
