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
    url: '/home/news',
    icon: 'fa fa-newspaper-o'
  }
];

export const guildNav: INavData[] = [

  {
    name: 'Overview',
    url: '/guild/:id/overview',
    icon: 'fa fa-hashtag'
  },
  {
    name: 'Bank',
    url: '/guild/:id/bank',
    icon: 'fa fa-bank',
    children: [
      {
        name: 'Overview',
        url: '/guild/:id/bank/overview',
        icon: 'fa fa-bar-chart'
      }
    ]
  }
];
