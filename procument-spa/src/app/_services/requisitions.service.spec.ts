/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RequisitionsService } from './requisitions.service';

describe('Service: Requisitions', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RequisitionsService]
    });
  });

  it('should ...', inject([RequisitionsService], (service: RequisitionsService) => {
    expect(service).toBeTruthy();
  }));
});
