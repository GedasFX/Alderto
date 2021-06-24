import Image from 'next/image';
import { useMemo } from 'react';
import discordLoader from 'src/util/loaders/discordLoader';

export default function NewsPost({
  authorAvatarId,
  authorUsername,
  contents,
  createdAt,
}: App.NewsPost) {
  const date = useMemo(() => new Date(createdAt).toDateString(), [createdAt]);

  return (
    <div className="intro-y col-span-12 md:col-span-6 xl:col-span-4 box">
      <div className="flex items-center border-b border-gray-200 dark:border-dark-5 px-5 py-4">
        <div className="w-10 h-10 flex-none">
          <Image
            className="rounded-full"
            loader={discordLoader}
            src={`/avatars/127805043146489856/${authorAvatarId}.jpg`}
            alt="Author profile picture"
            width={64}
            height={64}
          />
        </div>
        <div className="ml-3 mr-auto">
          <span className="font-medium">{authorUsername}</span>
          <div className="flex text-gray-600 truncate text-xs mt-0.5">{date}</div>
        </div>
      </div>
      <div className="p-5">
        <div className="text-gray-700 dark:text-gray-400 mt-2 whitespace-pre-wrap">{contents}</div>
      </div>
    </div>
  );
}
