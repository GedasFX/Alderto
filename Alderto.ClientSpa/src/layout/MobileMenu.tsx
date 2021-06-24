import clsx from 'clsx';
import { FiBarChart2, FiChevronDown } from 'react-icons/fi';
import Link from 'next/link';
import { useState } from 'react';
import Logo from './Logo';
import routes, { Route, RouteGroup, RouteLink } from 'src/conf/routes';

function MenuRouteLink({ route }: { route: RouteLink }) {
  return (
    <Link href={route.path}>
      <a className="menu">
        <div className="menu__icon">{route.icon}</div>
        <div className="menu__title">{route.name}</div>
      </a>
    </Link>
  );
}

function MenuRouteGroup({ route }: { route: RouteGroup }) {
  const [open, setOpen] = useState(false);

  return (
    <>
      <button className="menu w-full" onClick={() => setOpen(o => !o)}>
        <div className="menu__icon">{route.icon}</div>
        <div className="menu__title">
          {route.name}
          <FiChevronDown
            size={24}
            strokeWidth={1.5}
            className={clsx('menu__sub-icon', 'transform', open && 'rotate-180')}
          />
        </div>
      </button>
      <ul className={clsx(open && 'menu__sub-open')}>
        {route.children.map(c => (
          <li key={c.name}>
            <Link href={c.path}>
              <a className="menu">
                <div className="menu__icon">{c.icon}</div>
                <div className="menu__title">{c.name}</div>
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
    <div className={clsx('mobile-menu', 'md:hidden')}>
      <div className="mobile-menu-bar">
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
