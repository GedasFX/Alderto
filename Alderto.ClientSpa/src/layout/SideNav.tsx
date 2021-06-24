import clsx from 'clsx';
import { FiChevronDown, FiHome } from 'react-icons/fi';
import routes, { Route, RouteGroup, RouteLink } from 'src/conf/routes';
import Link from 'next/link';
import { useMemo, useState } from 'react';
import { useRouter } from 'next/dist/client/router';

function MenuRouteLink({ route, active }: { route: RouteLink; active?: boolean }) {
  return (
    <Link href={route.path}>
      <a className={clsx('side-menu w-full', active && 'side-menu--active')}>
        <div className="side-menu__icon">{route.icon}</div>
        <div className="side-menu__title">{route.name}</div>
      </a>
    </Link>
  );
}

function MenuRouteGroup({ route, currentPath }: { route: RouteGroup; currentPath?: string }) {
  const [open, setOpen] = useState(false);
  const active = useMemo(
    () => route.children.some(c => c.path === currentPath),
    [currentPath, route.children]
  );

  return (
    <>
      <button
        className={clsx('side-menu w-full', active && 'side-menu--active')}
        onClick={() => setOpen(o => !o)}
      >
        <div className="side-menu__icon">{route.icon}</div>
        <div className="side-menu__title">
          {route.name}
          <FiChevronDown
            size={24}
            strokeWidth={1.5}
            className={clsx('side-menu__sub-icon', 'transform', open && 'rotate-180')}
          />
        </div>
      </button>
      <ul className={clsx(open && 'side-menu__sub-open')}>
        {route.children.map(c => (
          <li key={c.name}>
            <MenuRouteLink route={c} active={c.path === currentPath} />
          </li>
        ))}
      </ul>
    </>
  );
}
function MenuRoute({ route, currentPath }: { route: Route; currentPath?: string }) {
  return route.type == 'link' ? (
    <MenuRouteLink route={route} active={currentPath === route.path} />
  ) : (
    <MenuRouteGroup route={route} currentPath={currentPath} />
  );
}

export default function SideNav() {
  const { pathname } = useRouter();

  return (
    <nav className="side-nav">
      <ul>
        {routes.map(r => (
          <li key={r.name}>
            <MenuRoute route={r} currentPath={pathname} />
          </li>
        ))}
      </ul>
    </nav>
  );
}
