import { FiChevronRight } from 'react-icons/fi';

export default function Breadcrumb() {
  return (
    <div className="-intro-x mr-auto breadcrumb">
      <a href="">Application</a>
      <FiChevronRight className="breadcrumb__icon" />
      <a href="" className="breadcrumb--active">
        Dashboard
      </a>
    </div>
  );
}
