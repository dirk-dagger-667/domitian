import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { ReactiveFormsModule, ControlContainer, FormGroup, FormControl } from '@angular/forms';
import { TITDto } from '../../contracts/titdto';
import { emptyTITDto } from 'src/app/core/factories/object-factories';

@Component({
  selector: 'app-text-input-titled',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './text-input-titled.component.html',
  styleUrl: './text-input-titled.component.css',
  viewProviders: [
    {
      provide: ControlContainer,
      useFactory: () => inject(ControlContainer, { skipSelf: true })
    }]
})
export class TextInputTitledComponent implements OnInit, OnDestroy
{

  @Input() titDto: TITDto = emptyTITDto;

  constructor(public parentContainer: ControlContainer) { }

  get parentFormGroup(): FormGroup
  {
    return this.parentContainer.control as FormGroup;
  }

  ngOnInit(): void
  {
    this.parentFormGroup.addControl(this.titDto.formCntrlName,
      new FormControl(this.titDto.initParams.placeholder, this.titDto.initParams.options));
  }

  ngOnDestroy()
  {
    this.parentFormGroup.removeControl(this.titDto.formCntrlName);
  }

}
