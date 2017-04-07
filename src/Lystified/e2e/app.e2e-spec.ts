import { LystifiedPage } from './app.po';

describe('lystified App', () => {
  let page: LystifiedPage;

  beforeEach(() => {
    page = new LystifiedPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
