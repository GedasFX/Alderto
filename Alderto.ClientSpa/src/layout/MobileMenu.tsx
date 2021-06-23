import clsx from 'clsx';
import { FiBarChart2, FiChevronDown, FiHome, FiActivity } from 'react-icons/fi';
import styles from './MobileMenu.module.css';
import Link from 'next/link';
import { useState } from 'react';
import Logo from './Logo';

type RouteCommon = {
  name: string;
  icon: JSX.Element;
};
type RouteLink = RouteCommon & {
  type: 'link';
  path: string;
};
type RouteGroup = RouteCommon & {
  type: 'group';
  children: RouteLink[];
};
type Route = RouteLink | RouteGroup;

const routes: Route[] = [
  {
    type: 'link',
    path: '/',
    name: 'Home',
    icon: <FiHome size={24} strokeWidth={1.5} />,
  },
  {
    type: 'group',
    name: 'Lome',
    icon: <FiHome size={24} strokeWidth={1.5} />,
    children: [
      {
        type: 'link',
        path: '/',
        name: 'Child #2',
        icon: <FiActivity size={24} strokeWidth={1.5} />,
      },
      {
        type: 'link',
        path: '/',
        name: 'Child #3',
        icon: <FiActivity size={24} strokeWidth={1.5} />,
      },
    ],
  },
];

function MenuRouteLink({ route }: { route: RouteLink }) {
  return (
    <Link href={route.path}>
      <a className={styles['menu']}>
        <div className={styles['menu__icon']}>{route.icon}</div>
        <div className={styles['menu__title']}>{route.name}</div>
      </a>
    </Link>
  );
}
function MenuRouteGroup({ route }: { route: RouteGroup }) {
  const [open, setOpen] = useState(false);

  return (
    <>
      <button className="w-full" onClick={() => setOpen(o => !o)}>
        <a className={styles['menu']}>
          <div className={styles['menu__icon']}>{route.icon}</div>
          <div className={styles['menu__title']}>
            {route.name}
            <FiChevronDown
              size={24}
              strokeWidth={1.5}
              className={clsx(styles['menu__sub-icon'], 'transform', open && 'rotate-180')}
            />
          </div>
        </a>
      </button>
      <ul className={clsx(open && styles['menu__sub-open'])}>
        {route.children.map(c => (
          <li key={c.name}>
            <Link href={c.path}>
              <a className={styles['menu']}>
                <div className={styles['menu__icon']}>{c.icon}</div>
                <div className={styles['menu__title']}>{c.name}</div>
              </a>
            </Link>
          </li>
        ))}
      </ul>
    </>
  );
}
function MenuRoute({ route }: { route: Route }) {
  return route.type == 'link' ? <MenuRouteLink route={route} /> : <MenuRouteGroup route={route} />;
}

export default function MobileMenu() {
  const [navVisible, setNavVisible] = useState(false);

  return (
    <div className={clsx(styles['mobile-menu'], 'md:hidden')}>
      <div className={clsx(styles['mobile-menu-bar'])}>
        <Link href="/">
          <a className="flex mr-auto">
            <Logo />
          </a>
        </Link>
        <button
          onClick={() => {
            setNavVisible(v => !v);
          }}
        >
          <FiBarChart2 className="w-8 h-8 text-white transform -rotate-90" />
        </button>
      </div>
      <ul
        className={clsx(
          'border-t border-theme-2',
          'max-h-screen overflow-hidden transition-all duration-300',
          navVisible && 'py-5'
        )}
        style={{
          maxHeight: !navVisible ? 0 : undefined,
        }}
      >
        {routes.map(r => (
          <li key={r.name}>
            <MenuRoute route={r} />
          </li>
        ))}
      </ul>
    </div>
  );
}
