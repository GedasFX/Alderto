import clsx from 'clsx';
import { FiChevronRight } from 'react-icons/fi';
import styles from './Breadcrumb.module.css';

export default function Breadcrumb() {
  return (
    <div className={clsx('-intro-x mr-auto', styles['breadcrumb'])}>
      <a href="">Application</a>
      <FiChevronRight className={styles['breadcrumb__icon']} />
      <a href="" className={styles['breadcrumb--active']}>
        Dashboard
      </a>
    </div>
  );
}
