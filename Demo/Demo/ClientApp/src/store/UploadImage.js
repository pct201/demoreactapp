
const showImageCropperPopupType = "BTN_UPLOAD_IMAGE_POPUP_CLICK";
const handleModelHideType = "MODEL_HIDE";
const uploadCroppedImageType = "BTN_UPLOAD_CROPPED_IMG_CLICK";

const initialState = {
    isShowImageCropperPopup: false,
    cropResult: null
}

export const actionCreators = {
    showImageCropperPopup: () => ({ type: showImageCropperPopupType }),
    handleModelHide: () => ({ type: handleModelHideType }),
    uploadCroppedImage: (image) => ({ type: uploadCroppedImageType,image})
}

export const reducer = (state, action) => { 
    state = state || initialState;

    if (action.type === showImageCropperPopupType) {
        return { ...state, isShowImageCropperPopup: true };
    }

    if (action.type === handleModelHideType) {
        return { ...state, isShowImageCropperPopup: false };
    }

    if (action.type === uploadCroppedImageType) {
        return {
            ...state,
            isShowImageCropperPopup: false,
            cropResult: action.image
        }
    }
    return state;
}