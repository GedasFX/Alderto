import { Component, OnInit } from '@angular/core';

export interface Documentation {
  lastUpdated: Date;
  modules: Module[];
}

export interface Module {
  name: string;
  commands: Command[];
}

export interface Command {
  trigger: string;
  alias: string[];
  params: Param[];
  requirements: string;
  notes: string;
  example: string;
}

export interface Param {
  name: string;
  type: string;
  multiple: boolean;
  defaultValue: string;
}

const docs = {
  lastUpdated: new Date(2020, 5, 1),
  modules: [
    {
      name: 'Points',
      commands: [
        {
          trigger: 'give',
          params: [
            {
              name: 'pointCount',
              type: 'integer'
            },
            {
              name: 'user',
              type: '@user',
              multiple: true
            }
          ],
          notes: 'Gives an amount of points to the listed users. Supports multiple users at a time.',
          requirements: '\'Admin\' Role',
          example: '.give 5 @SomeUser @SomeOtherUser'
        },
        {
          trigger: 'take',
          params: [
            {
              name: 'pointCount',
              type: 'integer'
            },
            {
              name: 'user',
              type: '@user',
              multiple: true
            }
          ],
          notes: 'Takes an amount of points from the listed users. Supports multiple users at a time.',
          requirements: '\'Admin\' Role',
          example: '.take 5 @SomeUser @SomeOtherUser'
        },
        {
          trigger: 'timely',
          alias: ['moon'],
          notes: 'Grants a timely currency reward. Cooldown and reward are both configured per guild.',
          example: '.timely'
        },
        {
          trigger: 'top',
          params: [
            {
              name: 'page',
              type: 'integer',
              defaultValue: '1'
            }
          ],
          notes: 'Gets the top 50 users in the guild. Supports pagination.',
          example: '.top 2'
        }
      ]
    },
    {
      name: 'Bank',
      commands: [
        {
          trigger: 'bank list',
          alias: ['gb list'],
          notes: 'Views all registered banks.',
          example: '.gb list'
        },
        {
          trigger: 'bank items $bankName',
          alias: ['gb items'],
          params: [
            {
              name: 'bankName',
              type: 'text'
            }
          ],
          notes: 'Gets the contents of the specified bank.',
          example: '.gb items "My favourite Bank"'
        }
      ]
    },
    {
      name: 'Preferences',
      commands: [
        {
          trigger: 'preferences get',
          alias: ['config get'],
          params: [
            {
              name: 'preferenceName',
              type: 'string'
            }
          ],
          notes: 'Gets the specified guild\'s preference. ' +
            'Valid preference names are \'prefix\', \'currencySymbol\', \'timelyCooldown\', \'timelyRewardQuantity\'',
          requirements: 'Administrator permission',
          example: '.config get prefix'
        },
        {
          trigger: 'preferences set',
          alias: ['config set'],
          params: [
            {
              name: 'preferenceName',
              type: 'string'
            },
            {
              name: 'value',
              type: 'string'
            }
          ],
          notes: 'Sets the specified guild\'s preference. ' +
            'Valid preference names are \'prefix\', \'currencySymbol\', \'timelyCooldown\', \'timelyRewardQuantity\'',
          requirements: 'Administrator permission',
          example: '.config set prefix ⚽'
        },
      ]
    }
  ]
} as Documentation;

@Component({
  templateUrl: './documentation.component.html'
})
export class DocumentationComponent implements OnInit {
  public docs = docs;

  constructor() { }

  public ngOnInit() {
  }
}
