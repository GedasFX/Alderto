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

export const navItems: INavData[] = [
  {
    name: 'Guild Bank',
    url: '/guild/:id/bank',
    icon: 'fa fa-bank',
    children: [
      {
        name: 'Overview',
        url: '/guild/:id/bank/overview',
        icon: 'fa fa-bar-chart',
        children: [
          {
            name: 'Overview',
            url: '/guild/:id/bank/overview',
            icon: 'fa fa-bar-chart',
            children: [
              {
                name: 'Overview',
                url: '/guild/:id/bank/overview',
                icon: 'fa fa-bar-chart'
              }
            ]
          },
          {
            name: 'Overview',
            url: '/guild/:id/bank/overview',
            icon: 'fa fa-bar-chart',
            children: [
              {
                name: 'Overview',
                url: '/guild/:id/bank/overview',
                icon: 'fa fa-bar-chart'
              }
            ]
          }
        ]
      }
    ]
  }
];
