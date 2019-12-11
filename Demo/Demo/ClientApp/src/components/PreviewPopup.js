import React from 'react';
import { Modal } from 'react-bootstrap';

const PreviewPopup = (props) => {
    return (
        <Modal className="bigPopup summernoteimg" show={props.show} aria-labelledby="contained-modal-title-vcenter" centered="true">
            <Modal.Body dangerouslySetInnerHTML={{ __html: props.summernoteContent }}>
                
            </Modal.Body>
            <Modal.Footer>
                <input type="button" className="cus-button primary" value="OK" onClick={props.popupClose}/>
            </Modal.Footer>
        </Modal>)
}

export default PreviewPopup;