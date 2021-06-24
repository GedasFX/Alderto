import clsx from 'clsx';
import { FiChevronDown } from 'react-icons/fi';
import styles from './SideNav.module.css';

export default function SideNav() {
  return (
    <nav className={styles['side-nav']}>
      <ul>
        <li>
          <a
            href="javascript:;.html"
            className={clsx(styles['side-menu'], styles['side-menu--active'])}
          >
            <div className={styles['side-menu__icon']}>
              <i data-feather="home"></i>
            </div> 
            <div className={styles['side-menu__title']}>
              Dashboard
              <div className={clsx(styles['side-menu__sub-icon'], 'transform rotate-180')}>
                <FiChevronDown />
              </div>
            </div>
          </a>
          <ul className="side-menu__sub-open">
            <li>
              <a
                href="side-menu-dark-dashboard-overview-1.html"
                className="side-menu side-menu--active"
              >
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Overview 1 </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-dashboard-overview-2.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Overview 2 </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-dashboard-overview-3.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Overview 3 </div>
              </a>
            </li>
          </ul>
        </li>
        <li>
          <a href="javascript:;" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="box"></i>
            </div>
            <div className="side-menu__title">
              Menu Layout
              <div className="side-menu__sub-icon ">
                <i data-feather="chevron-down"></i>
              </div>
            </div>
          </a>
          <ul className="">
            <li>
              <a href="side-menu-dark-dashboard-overview-1.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Side Menu </div>
              </a>
            </li>
            <li>
              <a href="simple-menu-dark-dashboard-overview-1.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Simple Menu </div>
              </a>
            </li>
            <li>
              <a href="top-menu-dark-dashboard-overview-1.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Top Menu </div>
              </a>
            </li>
          </ul>
        </li>
        <li>
          <a href="side-menu-dark-inbox.html" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="inbox"></i>
            </div>
            <div className="side-menu__title"> Inbox </div>
          </a>
        </li>
        <li>
          <a href="side-menu-dark-file-manager.html" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="hard-drive"></i>
            </div>
            <div className="side-menu__title"> File Manager </div>
          </a>
        </li>
        <li>
          <a href="side-menu-dark-point-of-sale.html" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="credit-card"></i>
            </div>
            <div className="side-menu__title"> Point of Sale </div>
          </a>
        </li>
        <li>
          <a href="side-menu-dark-chat.html" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="message-square"></i>
            </div>
            <div className="side-menu__title"> Chat </div>
          </a>
        </li>
        <li>
          <a href="side-menu-dark-post.html" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="file-text"></i>
            </div>
            <div className="side-menu__title"> Post </div>
          </a>
        </li>
        <li>
          <a href="side-menu-dark-calendar.html" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="calendar"></i>
            </div>
            <div className="side-menu__title"> Calendar </div>
          </a>
        </li>
        <li className="side-nav__devider my-6"></li>
        <li>
          <a href="javascript:;" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="edit"></i>
            </div>
            <div className="side-menu__title">
              Crud
              <div className="side-menu__sub-icon ">
                <i data-feather="chevron-down"></i>
              </div>
            </div>
          </a>
          <ul className="">
            <li>
              <a href="side-menu-dark-crud-data-list.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Data List </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-crud-form.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Form </div>
              </a>
            </li>
          </ul>
        </li>
        <li>
          <a href="javascript:;" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="users"></i>
            </div>
            <div className="side-menu__title">
              Users
              <div className="side-menu__sub-icon ">
                <i data-feather="chevron-down"></i>
              </div>
            </div>
          </a>
          <ul className="">
            <li>
              <a href="side-menu-dark-users-layout-1.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Layout 1 </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-users-layout-2.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Layout 2 </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-users-layout-3.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Layout 3 </div>
              </a>
            </li>
          </ul>
        </li>
        <li>
          <a href="javascript:;" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="trello"></i>
            </div>
            <div className="side-menu__title">
              Profile
              <div className="side-menu__sub-icon ">
                <i data-feather="chevron-down"></i>
              </div>
            </div>
          </a>
          <ul className="">
            <li>
              <a href="side-menu-dark-profile-overview-1.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Overview 1 </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-profile-overview-2.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Overview 2 </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-profile-overview-3.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Overview 3 </div>
              </a>
            </li>
          </ul>
        </li>
        <li>
          <a href="javascript:;" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="layout"></i>
            </div>
            <div className="side-menu__title">
              Pages
              <div className="side-menu__sub-icon ">
                <i data-feather="chevron-down"></i>
              </div>
            </div>
          </a>
          <ul className="">
            <li>
              <a href="javascript:;" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title">
                  Wizards
                  <div className="side-menu__sub-icon ">
                    <i data-feather="chevron-down"></i>
                  </div>
                </div>
              </a>
              <ul className="">
                <li>
                  <a href="side-menu-dark-wizard-layout-1.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 1</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-wizard-layout-2.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 2</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-wizard-layout-3.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 3</div>
                  </a>
                </li>
              </ul>
            </li>
            <li>
              <a href="javascript:;" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title">
                  Blog
                  <div className="side-menu__sub-icon ">
                    <i data-feather="chevron-down"></i>
                  </div>
                </div>
              </a>
              <ul className="">
                <li>
                  <a href="side-menu-dark-blog-layout-1.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 1</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-blog-layout-2.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 2</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-blog-layout-3.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 3</div>
                  </a>
                </li>
              </ul>
            </li>
            <li>
              <a href="javascript:;" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title">
                  Pricing
                  <div className="side-menu__sub-icon ">
                    <i data-feather="chevron-down"></i>
                  </div>
                </div>
              </a>
              <ul className="">
                <li>
                  <a href="side-menu-dark-pricing-layout-1.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 1</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-pricing-layout-2.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 2</div>
                  </a>
                </li>
              </ul>
            </li>
            <li>
              <a href="javascript:;" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title">
                  Invoice
                  <div className="side-menu__sub-icon ">
                    <i data-feather="chevron-down"></i>
                  </div>
                </div>
              </a>
              <ul className="">
                <li>
                  <a href="side-menu-dark-invoice-layout-1.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 1</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-invoice-layout-2.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 2</div>
                  </a>
                </li>
              </ul>
            </li>
            <li>
              <a href="javascript:;" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title">
                  FAQ
                  <div className="side-menu__sub-icon ">
                    <i data-feather="chevron-down"></i>
                  </div>
                </div>
              </a>
              <ul className="">
                <li>
                  <a href="side-menu-dark-faq-layout-1.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 1</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-faq-layout-2.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 2</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-faq-layout-3.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Layout 3</div>
                  </a>
                </li>
              </ul>
            </li>
            <li>
              <a href="login-dark-login.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Login </div>
              </a>
            </li>
            <li>
              <a href="login-dark-register.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Register </div>
              </a>
            </li>
            <li>
              <a href="main-dark-error-page.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Error Page </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-update-profile.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Update profile </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-change-password.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Change Password </div>
              </a>
            </li>
          </ul>
        </li>
        <li className="side-nav__devider my-6"></li>
        <li>
          <a href="javascript:;" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="inbox"></i>
            </div>
            <div className="side-menu__title">
              Components
              <div className="side-menu__sub-icon ">
                <i data-feather="chevron-down"></i>
              </div>
            </div>
          </a>
          <ul className="">
            <li>
              <a href="javascript:;" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title">
                  Table
                  <div className="side-menu__sub-icon ">
                    <i data-feather="chevron-down"></i>
                  </div>
                </div>
              </a>
              <ul className="">
                <li>
                  <a href="side-menu-dark-regular-table.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Regular Table</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-tabulator.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Tabulator</div>
                  </a>
                </li>
              </ul>
            </li>
            <li>
              <a href="javascript:;" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title">
                  Overlay
                  <div className="side-menu__sub-icon ">
                    <i data-feather="chevron-down"></i>
                  </div>
                </div>
              </a>
              <ul className="">
                <li>
                  <a href="side-menu-dark-modal.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Modal</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-slide-over.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Slide Over</div>
                  </a>
                </li>
                <li>
                  <a href="side-menu-dark-notification.html" className="side-menu">
                    <div className="side-menu__icon">
                      <i data-feather="zap"></i>
                    </div>
                    <div className="side-menu__title">Notification</div>
                  </a>
                </li>
              </ul>
            </li>
            <li>
              <a href="side-menu-dark-accordion.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Accordion </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-button.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Button </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-alert.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Alert </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-progress-bar.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Progress Bar </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-tooltip.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Tooltip </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-dropdown.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Dropdown </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-typography.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Typography </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-icon.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Icon </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-loading-icon.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Loading Icon </div>
              </a>
            </li>
          </ul>
        </li>
        <li>
          <a href="javascript:;" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="sidebar"></i>
            </div>
            <div className="side-menu__title">
              Forms
              <div className="side-menu__sub-icon ">
                <i data-feather="chevron-down"></i>
              </div>
            </div>
          </a>
          <ul className="">
            <li>
              <a href="side-menu-dark-regular-form.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Regular Form </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-datepicker.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Datepicker </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-tom-select.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Tom Select </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-file-upload.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> File Upload </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-wysiwyg-editor.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Wysiwyg Editor </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-validation.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Validation </div>
              </a>
            </li>
          </ul>
        </li>
        <li>
          <a href="javascript:;" className="side-menu">
            <div className="side-menu__icon">
              <i data-feather="hard-drive"></i>
            </div>
            <div className="side-menu__title">
              Widgets
              <div className="side-menu__sub-icon ">
                <i data-feather="chevron-down"></i>
              </div>
            </div>
          </a>
          <ul className="">
            <li>
              <a href="side-menu-dark-chart.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Chart </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-slider.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Slider </div>
              </a>
            </li>
            <li>
              <a href="side-menu-dark-image-zoom.html" className="side-menu">
                <div className="side-menu__icon">
                  <i data-feather="activity"></i>
                </div>
                <div className="side-menu__title"> Image Zoom </div>
              </a>
            </li>
          </ul>
        </li>
      </ul>
    </nav>
  );
}
