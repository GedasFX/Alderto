import Image from 'next/image';
import logo from 'public/images/logo.svg';

export default function Logo() {
  return <Image alt="Icewall Tailwind HTML Admin Template" src={logo} height={24} width={24} />;
}
