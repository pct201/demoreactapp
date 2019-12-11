import React, { Component } from 'react';
import ImageCropperPopup from './ImageCropperPopup';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreators } from '../store/UploadImage';

class UploadImage extends Component {

    render() {
        return (

            <div>
                <label >Profile Picture:</label>
                <div className="ci-box height130" id="cmp_logo_div">
                    <div className="company-logo">
                        <div style={{ "position": "relative" }}>
                            <a href="javascript:void(0);" className="fl-helplink" style={{ "top": "15px", "right": "-25px", "zIndex": "110" }} data-toggle="tooltip" data-container="body" data-placement="top" title="" data-original-title="Your logo will appear on select documentation issued by the platform. Upload an image of 400x138 pixels or less in jpeg, jpg, png, bmp, or gif format."></a>
                        </div>
                        <div className="file-select">
                            <a href="javascript:void(0);" className="ci-editlink cus-button secondary" onClick={this.props.showImageCropperPopup} id="aLoadImageCropper">UPLOAD LOGO</a>
                        </div>
                        <div className="ci-table">
                            <div>
                                <img src={this.props.cropResult != null ? this.props.cropResult : "@imgSrc"} name="6" alt="" />
                            </div>
                        </div>
                    </div>
                </div>
                <ImageCropperPopup show={this.props.isShowImageCropperPopup} popupClose={this.props.handleModelHide} imageSrc={this.props.cropResult != null ? this.props.cropResult : ""}
                    onUploadImage={(result) => { this.props.uploadCroppedImage(result) }} />
            </div>

        )
    }
}

function mapStatetoProps(state) {
    return {
        isShowImageCropperPopup: state.uploadImage.isShowImageCropperPopup,
        cropResult: state.uploadImage.cropResult
    }
}

export default connect(
    mapStatetoProps,
    dispatch => bindActionCreators(actionCreators, dispatch)
)(UploadImage)