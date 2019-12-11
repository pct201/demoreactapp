import React, { Component } from 'react';
import Cropper from 'react-cropper';
import 'cropperjs/dist/cropper.css';
import "../content/fontawesome/css/font-awesome.min.css";

export default class ImageCropper extends Component {

    constructor(props) {
        super(props);
        this.state = {
            src: props.imageSrc,
            imageReady: true
        };
        this.cropImage = this.cropImage.bind(this);
        this.onChange = this.onChange.bind(this);
        this.croppedImage = this.croppedImage.bind(this);
    }

    onChange(e) {
        e.preventDefault();
        this.setState({ imageReady: false });
        let files;
        if (e.dataTransfer) {
            files = e.dataTransfer.files;
        } else if (e.target) {
            files = e.target.files;
        }
        const reader = new FileReader();
        reader.onload = () => {
            this.setState({ src: reader.result, imageReady: true });
        };
        if (files != null) {
            reader.readAsDataURL(files[0]);
        }
    }

    cropImage() {
        if (typeof this.cropper === 'undefined' || this.refs.btnUploadCroppedImage.classList.contains("disabled")) {
            return;
        }
        const cropResult = this.cropper.getCroppedCanvas().toDataURL();
        this.props.onUploadImage(cropResult);
    }

    _crop() {
        let width = this.cropper.getCroppedCanvas().width;
        let height = this.cropper.getCroppedCanvas().height;
        this.refs.dataWidth.value = width;
        this.refs.dataHeight.value = height;
        if (width <= 400 && height <= 138) {
            this.refs.btnUploadCroppedImage.classList.remove("disabled");
            this.refs.dataWidth.classList.remove("input-validation-error");
            this.refs.dataHeight.classList.remove("input-validation-error");
        }
        else {
            this.refs.btnUploadCroppedImage.classList.add("disabled");
            this.refs.dataWidth.classList.add("input-validation-error");
            this.refs.dataHeight.classList.add("input-validation-error");
        }
    }

    croppedImage(result) {
        this.props.onUploadImage(result);
    }

    onZoomIn() {
        this.cropper.zoom(0.1);
    }

    onZoomOut() {
        this.cropper.zoom(-0.1);
    }

    onRotateLeft() {
        this.cropper.rotate(-45);
    }

    onRotateRight() {
        this.cropper.rotate(45);
    }

    onReset() {
        this.cropper.reset();
    }

    onCrop() {
        this.cropper.setDragMode('crop');
    }

    onMove() {
        this.cropper.setDragMode('move');
    }

    render() {
        let prepareCropper = null;
        if (this.state.imageReady) {
            prepareCropper = (
                <Cropper
                    style={{ height: 400, width: '100%' }}
                    aspectRatio={16 / 9}
                    preview=".img-preview"
                    guides={true}
                    src={this.state.src}
                    ref={cropper => { this.cropper = cropper; }}
                    crop={this._crop.bind(this)}
                />
            )
        };
        return (
            <div>
                <div className="cf-body img-editpopup">
                    <div className="clearfix">
                        <p className="img-upload-msg text-right"> <span id="spnErrMsg"> Please ensure your image file is 400x138 pixels or less.</span></p>
                        <div className="img-container cropper-bg">
                            {prepareCropper}
                        </div>
                    </div>
                    <br />
                    <div className="clearfix">
                        <div className="docs-buttons">
                            <button type="button" className="btn cus-button secondary" onClick={(e) => this.onMove()} title="Move">
                                <span className="fa fa-arrows"></span>
                            </button>
                            <button type="button" className="btn cus-button secondary" onClick={(e) => this.onCrop()} title="Crop">
                                <span className="fa fa-crop"></span>
                            </button>
                            <button type="button" className="btn cus-button secondary" onClick={(e) => this.onZoomIn()} title="Zoom In">
                                <span className="fa fa-search-plus"></span>
                            </button>
                            <button type="button" className="btn cus-button secondary" onClick={(e) => this.onZoomOut()} title="Zoom Out">
                                <span className="fa fa-search-minus"></span>
                            </button>
                            <button type="button" className="btn cus-button secondary" onClick={(e) => this.onRotateLeft()} title="Rotate Left">
                                <span className="fa fa-rotate-left"></span>
                            </button>
                            <button type="button" className="btn cus-button secondary" onClick={(e) => this.onRotateRight()} title="Rotate Right">
                                <span className="fa fa-rotate-right"></span>
                            </button>
                            <button type="button" className="btn cus-button secondary" onClick={(e) => this.onReset()}>
                                <span className="fa fa-refresh"></span>
                            </button>
                            <div className="uploader-mainbtn">
                                <label className="btn cus-button primary" title="Upload image file">
                                    <input type="file" onChange={this.onChange} className="sr-only cropper-input-file" id="inputImage" name="file" accept="image/*" />
                                    Browse
                            </label>
                                <button type="button" id="btnUploadCroppedImage" ref="btnUploadCroppedImage" onClick={this.cropImage} className={this.props.imageSrc === "" ? "cus-button primary disabled" : "cus-button primary" } style={{ "cursor": "pointer" }} data-method="getCroppedCanvas" data-option="{ & quot;maxWidth&quot;: 4096, &quot;maxHeight&quot;: 4096 }">
                                    Upload
                        </button>
                            </div>
                        </div>
                        <div className="docs-preview">
                            <div className="img-preview preview-lg"></div>
                            <div className="clearfix"></div>
                            <div className="row row-mr15">
                                <div className="col-xs-6">
                                    <div className="field field--not-empty">
                                        <label className="field-label">Width(px)</label>
                                        <input type="text" className="cus-control field-input" ref="dataWidth" placeholder="width" disabled="disabled" />
                                    </div>
                                </div>
                                <div className="col-xs-6">
                                    <div className="field field--not-empty">
                                        <label className="field-label">Height(px)</label>
                                        <input type="text" className="cus-control field-input" ref="dataHeight" placeholder="height" disabled="disabled" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                 
                </div>
            </div>
        );
    }
}
