import clsx from 'clsx';
import { useState } from 'react';
import { usePopper } from 'react-popper';
import Image from 'next/image';
import discordLoader from 'src/util/loaders/discordLoader';

export default function AccountMenu() {
  const [referenceElement, setReferenceElement] = useState<HTMLElement | null>();
  const [popperElement, setPopperElement] = useState<HTMLElement | null>();
  const { styles } = usePopper(referenceElement, popperElement, {
    placement: 'bottom-end',
  });

  const [dropdownVisible, setDropdownVisible] = useState(false);

  return (
    <div className="intro-x dropdown w-8 h-8">
      <button
        ref={setReferenceElement}
        className="dropdown-toggle w-8 h-8 rounded-full overflow-hidden shadow-lg image-fit zoom-in scale-110"
        onClick={() => setDropdownVisible(v => !v)}
      >
        <Image
          alt="Icewall Tailwind HTML Admin Template"
          loader={discordLoader}
          src="/assets/dd4dbc0016779df1378e7812eabaa04d.png"
          width={32}
          height={32}
        />
      </button>
      <div
        ref={setPopperElement}
        style={styles.popper}
        className={clsx('dropdown-menu w-56', dropdownVisible && 'show')}
      >
        <div className="dropdown-menu__content box bg-theme-11 dark:bg-dark-6 text-white">
          <div className="p-4 border-b border-theme-12 dark:border-dark-3">
            <div className="font-medium">Sylvester Stallone</div>
            <div className="text-xs text-theme-13 mt-0.5 dark:text-gray-600">DevOps Engineer</div>
          </div>
          <div className="p-2">
            <a
              href=""
              className="flex items-center p-2 transition duration-300 ease-in-out hover:bg-theme-1 dark:hover:bg-dark-3 rounded-md"
            >
              <i data-feather="user" className="w-4 h-4 mr-2"></i> Profile
            </a>
            <a
              href=""
              className="flex items-center p-2 transition duration-300 ease-in-out hover:bg-theme-1 dark:hover:bg-dark-3 rounded-md"
            >
              <i data-feather="edit" className="w-4 h-4 mr-2"></i> Add Account
            </a>
            <a
              href=""
              className="flex items-center p-2 transition duration-300 ease-in-out hover:bg-theme-1 dark:hover:bg-dark-3 rounded-md"
            >
              <i data-feather="lock" className="w-4 h-4 mr-2"></i> Reset Password
            </a>
            <a
              href=""
              className="flex items-center p-2 transition duration-300 ease-in-out hover:bg-theme-1 dark:hover:bg-dark-3 rounded-md"
            >
              <i data-feather="help-circle" className="w-4 h-4 mr-2"></i> Help
            </a>
          </div>
          <div className="p-2 border-t border-theme-12 dark:border-dark-3">
            <a
              href=""
              className="flex items-center p-2 transition duration-300 ease-in-out hover:bg-theme-1 dark:hover:bg-dark-3 rounded-md"
            >
              <i data-feather="toggle-right" className="w-4 h-4 mr-2"></i> Logout
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}
