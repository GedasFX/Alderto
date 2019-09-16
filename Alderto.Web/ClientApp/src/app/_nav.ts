interface INavAttributes {
  [propName: string]: any;
}
interface INavWrapper {
  attributes: INavAttributes;
  element: string;
}
interface INavBadge {
  text: string;
  variant: string;
}
interface INavLabel {
  class?: string;
  variant: string;
}

export interface INavData {
  name?: string;
  url?: string;
  icon?: string;
  badge?: INavBadge;
  title?: boolean;
  children?: INavData[];
  variant?: string;
  attributes?: INavAttributes;
  divider?: boolean;
  class?: string;
  label?: INavLabel;
  wrapper?: INavWrapper;
}

export const homeNav: INavData[] = [
  {
    name: 'News',
    url: '/news',
    icon: 'fa fa-newspaper-o'
  },
  { /* Guild data import */ },
  {
    name: 'Documentation',
    url: '/documentation',
    icon: 'fa fa-file-code-o'
  },
  {
    name: 'Invite to server',
    url: 'https://discordapp.com/api/oauth2/authorize?client_id=595936593592844306&permissions=8&scope=bot',
    icon: 'fa fa-send'
  }
];

export const guildNav: INavData =
{
  name: 'Guild',
  url: '/guild/:id',
  icon: 'fa fa-group',
  children: [
    {
      name: 'Overview',
      url: '/guild/:id/overview',
      icon: 'fa fa-hashtag'
    },
    {
      name: 'Bank',
      icon: 'fa fa-bank',
      url: '/guild/:id/bank',
    }
  ]
};
