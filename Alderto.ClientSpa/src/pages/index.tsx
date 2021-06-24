import { GetStaticProps } from 'next';
import NewsPost from 'src/components/home/NewsPost';

export type HomeProps = {
  news: App.NewsPost[];
};
export default function Home({ news }: HomeProps) {
  return (
    <div>
      <div className="intro-y flex flex-col sm:flex-row items-center mt-8 mb-6">
        <h2 className="text-lg font-medium mr-auto">Latest news</h2>
      </div>
      <div className="grid grid-cols-1 space-y-4">
        {news.map(n => (
          <NewsPost key={n.createdAt} {...n} />
        ))}
      </div>
    </div>
  );
}

export const getStaticProps: GetStaticProps<HomeProps> = async () => {
  return {
    props: {
      news: [
        {
          id: '854795793559322664',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2021-06-16T18:53:18.522+00:00',
          contents:
            'Release 1.2.2:\n* Fixed reverse proxy headers not being respected. Again.\n+ Add Docker support',
        },
        {
          id: '728260226024865812',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2020-07-02T14:46:08.305+00:00',
          contents: 'Release 1.2.1:\n* Fixed reverse proxy headers not being respected.',
        },
        {
          id: '705701970966741043',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2020-05-01T08:47:41.187+00:00',
          editedAt: '2020-05-01T08:59:22.037+00:00',
          contents:
            'Release 1.2.0:\n\n* Added leaderboards (command .top).\n* Added command reference ( \'/documentation\' )\n* Added ability to modify preferences from website\n* Improved banks (.banks list & .banks items "Bank Name")\n* Fixed login to website.',
        },
        {
          id: '680136483117596674',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2020-02-20T19:39:33.9+00:00',
          contents:
            'Release 1.1.0:\n\n* Added support for OAuth flow.\n* Added support for session refreshing.',
        },
        {
          id: '635564637802463272',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2019-10-20T19:46:57.872+00:00',
          editedAt: '2019-10-20T19:52:23.972+00:00',
          contents:
            'Pre-Release 1.0.0rc1:\n  * Add final prerelease feature - Managed messages!\n    & Editable by anyone, who is authorized (authorization is customizable on role basis)\n    & No more nagging other people to update a post!\n\n  * Bugfix - changing server now updates the contents accordingly.\n\n  * Dev changes - backend stability changes (error handling, nullable reference types, drastically increased code coverage, etc.)',
        },
        {
          id: '628324015219408903',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2019-09-30T20:15:18.924+00:00',
          contents:
            'Release 0.4.0:\n  Upgrade project to .Net Core 3.0\n  Add Moderator role Id to clan bank',
        },
        {
          id: '623150956053528597',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2019-09-16T13:39:25.508+00:00',
          contents:
            'Release 0.3.0:\n  Moved migrations to a better place.\n  Improved development conveyor for faster updates.',
        },
        {
          id: '622421227415404544',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2019-09-14T13:19:44.643+00:00',
          editedAt: '2019-09-14T13:21:55.574+00:00',
          contents:
            'Release 0.2.3:\n  Added News page.\n  Modified deployment key handling in backend.\n  Removed ugly logo on mobile.',
        },
        {
          id: '622411936130269185',
          channelId: '622411693376405521',
          authorUsername: 'GedasFX',
          authorAvatarId: '1717076eeecac479b56ff0867f46dcf6',
          createdAt: '2019-09-14T12:42:49.428+00:00',
          contents: 'Established bot control. Now bot is working!',
        },
      ],
    },
  };
};
