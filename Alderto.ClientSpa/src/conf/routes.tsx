import { FiHome, FiActivity, FiNavigation, FiFolder } from 'react-icons/fi';

export type RouteCommon = {
  name: string;
  icon: JSX.Element;
};

export type RouteLink = RouteCommon & {
  type: 'link';
  path: string;
};

export type RouteGroup = RouteCommon & {
  type: 'group';
  children: RouteLink[];
};

export type Route = RouteLink | RouteGroup;

const routes: Route[] = [
  {
    type: 'link',
    path: '/',
    name: 'News',
    icon: <FiHome size={24} strokeWidth={1.5} />,
  },
  {
    type: 'link',
    path: '/docs',
    name: 'Docs',
    icon: <FiFolder size={24} strokeWidth={1.5} />,
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
        path: '/foo',
        name: 'Child #3',
        icon: <FiActivity size={24} strokeWidth={1.5} />,
      },
    ],
  },
  {
    type: 'group',
    name: 'Lomesssss',
    icon: <FiHome size={24} strokeWidth={1.5} />,
    children: [
      {
        type: 'link',
        path: '/foo',
        name: 'Child #2',
        icon: <FiActivity size={24} strokeWidth={1.5} />,
      },
      {
        type: 'link',
        path: '/bar',
        name: 'Child #3',
        icon: <FiActivity size={24} strokeWidth={1.5} />,
      },
    ],
  },
  {
    type: 'link',
    path: 'https://discordapp.com/api/oauth2/authorize?client_id=595936593592844306&permissions=8&scope=bot',
    name: 'Invite to Server',
    icon: <FiNavigation size={24} strokeWidth={1.5} />,
  },
];

export default routes;
