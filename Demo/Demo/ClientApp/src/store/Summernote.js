
const handleSummernoteChange = "SUMMERNOTE_CHANGE"
const handlePreviewClick = "SUMMERNOTE_PREVIEW_CLICK"
const handleModelHide = "MODEL_HIDE"

const initialState = {
    summernoteContent: "",
    isShow: false
};

export const actionCreatorsSummernote = {
    SummernoteChange: (content) => ({ type: handleSummernoteChange, content }),
    PreviewClick: () => ({ type: handlePreviewClick }),
    handleModelHide: () => ({ type: handleModelHide })
};

export const reducer = (state, action) => {
    state = state || initialState;

    if (action.type === handleSummernoteChange) {
        return { ...state, summernoteContent: action.content };
    }

    if (action.type === handlePreviewClick) {
        return { ...state, isShow: true };
    }

    if (action.type === handleModelHide) {
        return { ...state, isShow: false };
    }

    return state;
};