import MobileDetect from 'mobile-detect';
import detectHover from 'detect-hover';
import constants from './constants';
import viewport from './viewport';

export const DEVICE_TYPE_DESKTOP_WIDE = 'desktop-wide';
export const DEVICE_TYPE_DESKTOP = 'desktop';
export const DEVICE_TYPE_TABLET = 'tablet';
export const DEVICE_TYPE_MOBILE_WIDE = 'mobile-wide';
export const DEVICE_TYPE_MOBILE = 'mobile';

class Device {
    constructor(viewports) {
        this.viewports = viewports;

        this.viewportType = null;
        this.viewportTypeIndex = null;

        const md = new MobileDetect(window.navigator.userAgent);
        this.isTouch = (md.mobile() !== null) && !detectHover.anyHover;
        if (this.isTouch) {
            document.documentElement.classList.remove('can-hover');
        }

        this.updateDeviceData();
        viewport.subscribeOnResize(this.onViewportResize);

        this.deviceTypeChangeCallbacks = [];
    }

    _isGreaterOrEqual(pixelsValue) {
        return matchMedia(`only screen and (min-width:${pixelsValue}px)`).matches;
    }

    updateDeviceData() {
        const viewportTypes = Object.keys(this.viewports);
        // reverse list because of mobile-first approach
        const viewportType = [...viewportTypes].reverse().find(key => this._isGreaterOrEqual(this.viewports[key]));
        this.viewportType = viewportType;
        // find index within original list of viewports
        this.viewportTypeIndex = viewportTypes.indexOf(viewportType);
    }

    onViewportResize = () => {
        const oldDeviceType = this.viewportType;

        this.updateDeviceData();

        if (oldDeviceType !== this.viewportType) {
            this.invokeDeviceTypeChange(this.viewportType, oldDeviceType);
        }
    };

    invokeDeviceTypeChange(newDeviceType, oldDeviceType) {
        this.deviceTypeChangeCallbacks.forEach(cb => cb(newDeviceType, oldDeviceType));
    }

    subscribeOnDeviceTypeChange(cb, getInitialValue = false) {
        this.deviceTypeChangeCallbacks.push(cb);
        if (getInitialValue) {
            cb(this.viewportType);
        }

        // return unsubscribe method
        return () => {
            this.deviceTypeChangeCallbacks = this.deviceTypeChangeCallbacks.filter(storedCb => storedCb !== cb);
        };
    }

    isViewportTypeMatch(deviceTypeName) {
        return this.viewportType === deviceTypeName;
    }

    getViewportTypeIndex(viewportType) {
        return Object.keys(this.viewports).findIndex(type => type === viewportType);
    }

    isViewportTypeGt(deviceTypeName) {
        return this.viewportTypeIndex > this.getViewportTypeIndex(deviceTypeName);
    }

    isViewportTypeGe(deviceTypeName) {
        return this.viewportTypeIndex >= this.getViewportTypeIndex(deviceTypeName);
    }

    isViewportTypeLt(deviceTypeName) {
        return this.viewportTypeIndex < this.getViewportTypeIndex(deviceTypeName);
    }

    isViewportTypeLe(deviceTypeName) {
        return this.viewportTypeIndex <= this.getViewportTypeIndex(deviceTypeName);
    }

    isGreaterOrEqualCustomWidth(value) {
        return this._isGreaterOrEqual(value);
    }
}

const viewports = {
    [DEVICE_TYPE_MOBILE]: 0,
    [DEVICE_TYPE_MOBILE_WIDE]: constants.VIEWPORT_WIDTH_MOBILE_WIDE,
    [DEVICE_TYPE_TABLET]: constants.VIEWPORT_WIDTH_TABLET,
    [DEVICE_TYPE_DESKTOP]: constants.VIEWPORT_WIDTH_DESKTOP,
    [DEVICE_TYPE_DESKTOP_WIDE]: constants.VIEWPORT_WIDTH_DESKTOP_WIDE,
};

const instance = new Device(viewports);
export default instance;
