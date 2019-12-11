import React, { Component } from 'react';
import { Modal } from 'react-bootstrap';
import ImageCropper from './ImageCropper';

class ImageCropperPopup extends Component {

    render() {
        return (
            <Modal show={this.props.show} id="imgcrop-popup" size="lg" aria-labelledby="contained-modal-title-vcenter" centered="true">
                <Modal.Header>
                    <Modal.Title>Profile Picture</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <ImageCropper onUploadImage={(result) => { this.props.onUploadImage(result) }} imageSrc={this.props.imageSrc} />
                </Modal.Body>
                <Modal.Footer>
                    <input type="button" className="cus-button primary" value="OK" onClick={this.props.popupClose} />
                </Modal.Footer>
            </Modal>
        )
    }
}

export default ImageCropperPopup;