import React, { Component } from 'react';
import { Button } from 'react-bootstrap';
import ImageCropper from './ImageCropper';

class ImageCropperPopup extends Component {

    render() { 
        let showModalClass = this.props.show ? "modal fade show" : "modal fade";
        return (
            <div className={showModalClass} id="WelcomeModal" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div className="modal-dialog modal-lg modal-dialog-centered" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" onClick={this.props.popupClose}>
                                <span aria-hidden="true">×</span>
                                <span className="sr-only">Close</span>
                            </button>
                            <h4 className="modal-title">Profile Picture</h4>
                        </div>
                        <div className="modal-body">
                            <ImageCropper onUploadImage={(result) => { this.props.onUploadImage(result) }} />
                        </div>
                        <div className="modal-footer">
                            <Button onClick={this.props.popupClose}>Close</Button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default ImageCropperPopup;